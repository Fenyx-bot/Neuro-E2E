import { useEffect, useState } from "react";
import { ChatResponseDto } from "../types";
import { getChats } from "../api/chat";
import { useNavigate } from "react-router-dom";
import { FiClock, FiMessageCircle, FiPlus } from "react-icons/fi";

function Home() {
  const [chats, setChats] = useState<ChatResponseDto[]>([]);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchChats = async () => {
      try {
        const response = await getChats();
        setChats(response);
      } catch (error) {
        console.error("Error fetching chats");
      }
    };
    fetchChats();
  }, []);

  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    const now = new Date();
    const diff = now.getTime() - date.getTime();
    const days = Math.floor(diff / (1000 * 60 * 60 * 24));

    if (days === 0) {
      return date.toLocaleTimeString([], {
        hour: "2-digit",
        minute: "2-digit",
      });
    } else if (days === 1) {
      return "Yesterday";
    } else if (days < 7) {
      return date.toLocaleDateString([], { weekday: "long" });
    } else {
      return date.toLocaleDateString([], { month: "short", day: "numeric" });
    }
  };

  return (
    <div className="container mx-auto max-w-4xl px-4 py-6">
      <div className="bg-white rounded-lg shadow-lg">
        {/* Header */}
        <div className="px-6 py-4 border-b border-gray-200 flex justify-between items-center">
          <div className="flex items-center space-x-2">
            <FiMessageCircle className="h-6 w-6 text-indigo-600" />
            <h1 className="text-2xl font-semibold text-gray-800">Your Chats</h1>
          </div>
          <button
            className="flex items-center space-x-1 px-4 py-2 bg-indigo-600 text-white rounded-lg hover:bg-indigo-700 transition-colors"
            onClick={() => navigate("/new-chat")}
          >
            <FiPlus className="h-5 w-5" />
            <span>New Chat</span>
          </button>
        </div>

        {/* Chat List */}
        <div className="divide-y divide-gray-200">
          {chats.length === 0 ? (
            <div className="p-8 text-center text-gray-500">
              <FiMessageCircle className="h-12 w-12 mx-auto mb-4 text-gray-400" />
              <p className="text-lg font-medium">No chats yet</p>
              <p className="mt-1">Start a new conversation to begin chatting</p>
            </div>
          ) : (
            chats.map((chat) => (
              <div
                key={chat.id}
                className="p-4 hover:bg-gray-50 cursor-pointer transition-colors"
                onClick={() => navigate(`/chat/${chat.id}`)}
              >
                <div className="flex justify-between items-start">
                  <div className="flex-1">
                    <h2 className="text-lg font-semibold text-gray-900">
                      {chat.name}
                    </h2>
                    <div className="mt-1 flex items-center space-x-4 text-sm text-gray-500">
                      <span>
                        {chat.user1_name} • {chat.user2_name}
                      </span>
                    </div>
                  </div>
                  <div className="flex items-center text-sm text-gray-500">
                    <FiClock className="h-4 w-4 mr-1" />
                    {formatDate(chat.last_message_at)}
                  </div>
                </div>
              </div>
            ))
          )}
        </div>
      </div>
    </div>
  );
}

export default Home;
