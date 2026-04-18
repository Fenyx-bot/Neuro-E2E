import { HubConnectionBuilder, HubConnectionState } from "@microsoft/signalr";
import { useState, useEffect, useRef } from "react";
import { MessageResponseDto } from "../types"; // Create this interface
import { getChatMessages } from "../api/chat";
import { useParams } from "react-router-dom";
import { MdSend } from "react-icons/md";
import { TbLoader2 } from "react-icons/tb";
import { useAuth } from "../context/AuthContext";

const secretKey = "ddZnpw61uhAuIaUjl73t4J0lzprx8T/eb52XuTj7MsI=";

async function encryptMessage(message: string) {
  try {
    // Decode the Base64 secret key into a Uint8Array
    const key = Uint8Array.from(atob(secretKey), (c) => c.charCodeAt(0));

    // Generate a random 16-byte IV
    const iv = window.crypto.getRandomValues(new Uint8Array(16));

    // Encode the message as a Uint8Array
    const encoder = new TextEncoder();
    const encodedMessage = encoder.encode(message);

    // Import the secret key for AES-CBC encryption
    const cryptoKey = await window.crypto.subtle.importKey(
      "raw",
      key,
      { name: "AES-CBC" },
      false,
      ["encrypt"],
    );

    // Encrypt the message
    const encrypted = await window.crypto.subtle.encrypt(
      {
        name: "AES-CBC",
        iv: iv,
      },
      cryptoKey,
      encodedMessage,
    );

    // Combine the IV and encrypted message for transmission
    const combined = new Uint8Array(iv.length + encrypted.byteLength);
    combined.set(iv); // First 16 bytes are the IV
    combined.set(new Uint8Array(encrypted), iv.length); // Remaining bytes are the ciphertext

    // Base64 encode the combined result
    return btoa(String.fromCharCode(...combined));
  } catch (err) {
    console.error("Encryption error:", err);
    throw new Error("Failed to encrypt the message");
  }
}

async function decryptMessage(encryptedMessage: string, secretKey: string) {
  try {
    // Decode the Base64 secret key into a Uint8Array
    const key = Uint8Array.from(atob(secretKey), (c) => c.charCodeAt(0));

    // Decode the Base64 encrypted message into a Uint8Array
    const cipherBytes = Uint8Array.from(atob(encryptedMessage), (c) =>
      c.charCodeAt(0),
    );

    // Extract the IV (first 16 bytes)
    const iv = cipherBytes.slice(0, 16);

    // Extract the actual encrypted content (remaining bytes)
    const encryptedContent = cipherBytes.slice(16);

    // Import the secret key for AES-CBC decryption
    const cryptoKey = await window.crypto.subtle.importKey(
      "raw",
      key,
      { name: "AES-CBC" },
      false,
      ["decrypt"],
    );

    // Decrypt the message
    const decrypted = await window.crypto.subtle.decrypt(
      {
        name: "AES-CBC",
        iv: iv,
      },
      cryptoKey,
      encryptedContent,
    );

    // Decode the decrypted content into a string
    const decoder = new TextDecoder();
    return decoder.decode(decrypted);
  } catch (err) {
    console.error("Decryption error:", err);
    throw new Error("Failed to decrypt the message");
  }
}

export const ChatPage = () => {
  const { chatId } = useParams();
  const [message, setMessage] = useState("");
  const [messages, setMessages] = useState<MessageResponseDto[]>([]);
  const [connection, setConnection] = useState<signalR.HubConnection | null>(
    null,
  );
  const [isConnecting, setIsConnecting] = useState(true);
  const messagesEndRef = useRef<HTMLDivElement>(null);
  const { user } = useAuth();

  const scrollToBottom = () => {
    messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
  };

  useEffect(() => {
    scrollToBottom();
  }, [messages]);

  useEffect(() => {
    const getMessages = async () => {
      try {
        const response = await getChatMessages(chatId as string);

        const decryptedMessages = await Promise.all(
          response.map(async (msg) => {
            const decryptedContent = await decryptMessage(
              msg.encrypted_content,
              secretKey,
            );
            console.log("Sender: ", msg.sender_username);
            console.log("Decrypted content: ", decryptedContent);

            // Return a new object with the decrypted content
            return {
              ...msg,
              encrypted_content: decryptedContent,
            };
          }),
        );

        setMessages(decryptedMessages);
      } catch (error) {
        console.error("Error fetching messages: ", error);
      }
    };

    const newConnection = new HubConnectionBuilder()
      .withUrl("http://localhost:5299/chathub", {
        accessTokenFactory: () => localStorage.getItem("token") || "",
      })
      .withAutomaticReconnect()
      .build();

    newConnection.on("ReceiveMessage", async (message: MessageResponseDto) => {
      message.encrypted_content = await decryptMessage(
        message.encrypted_content,
        secretKey,
      );

      setMessages((prevMessages) => [...prevMessages, message]);
    });

    newConnection
      .start()
      .then(async () => {
        console.log("Connected to SignalR hub!");
        newConnection.invoke("JoinChat", chatId);
        setConnection(newConnection);
        setIsConnecting(false);
        await getMessages();
      })
      .catch((err) => {
        console.error("Connection failed: ", err);
        setIsConnecting(false);
      });

    return () => {
      if (newConnection) {
        newConnection.stop();
      }
    };
  }, [chatId]);

  const sendMessage = async (e?: React.FormEvent) => {
    e?.preventDefault();

    if (!message.trim()) return;

    const encryptedMessage = await encryptMessage(message);
    if (connection && connection.state === HubConnectionState.Connected) {
      try {
        await connection.invoke("SendMessage", chatId, encryptedMessage);
        setMessage("");
      } catch (err) {
        console.error("Sending message failed: ", err);
      }
    }
  };

  return (
    <div className="container mx-auto max-w-4xl px-4 py-6">
      <div className="bg-white rounded-lg shadow-lg h-[calc(100vh-8rem)]">
        {/* Chat Header */}
        <div className="px-6 py-4 border-b border-gray-200">
          <h2 className="text-xl font-semibold text-gray-800">Chat Room</h2>
        </div>

        {/* Messages Area */}
        <div className="flex flex-col h-[calc(100%-8rem)]">
          <div className="flex-1 p-4 overflow-y-auto space-y-4">
            {isConnecting ? (
              <div className="flex items-center justify-center h-full">
                <TbLoader2 className="h-8 w-8 text-indigo-600 animate-spin" />
              </div>
            ) : (
              messages.map((msg, index) => (
                <div
                  key={index}
                  className={`flex ${
                    msg.sender_id === user?.id ? "justify-end" : "justify-start"
                  }`}
                >
                  <div
                    className={`max-w-[70%] px-4 py-2 rounded-lg ${
                      msg.sender_id === user?.id
                        ? "bg-indigo-600 text-white"
                        : "bg-gray-100 text-gray-800"
                    }`}
                  >
                    {
                      // Show the sender's username if it's not the current user
                      msg.sender_id != user?.id && (
                        <p className="text-xs font-medium mb-1">
                          {msg.sender_username}
                        </p>
                      )
                    }
                    <p className="text-sm">{msg.encrypted_content}</p>
                  </div>
                </div>
              ))
            )}
            <div ref={messagesEndRef} />
          </div>

          {/* Message Input */}
          <form
            onSubmit={sendMessage}
            className="p-4 border-t border-gray-200 bg-white"
          >
            <div className="flex space-x-2">
              <input
                type="text"
                value={message}
                onChange={(e) => setMessage(e.target.value)}
                placeholder="Type your message..."
                className="flex-1 px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-transparent outline-none"
              />
              <button
                type="submit"
                disabled={!connection || !message.trim()}
                className="px-4 py-2 bg-indigo-600 text-white rounded-lg hover:bg-indigo-700 disabled:opacity-50 disabled:cursor-not-allowed flex items-center space-x-2"
              >
                <MdSend className="h-5 w-5" />
                <span>Send</span>
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default ChatPage;
