import { ChatResponseDto, MessageResponseDto } from "../types";
import { toastError, toastSuccess } from "../utils/notify";
import api from "./api";

export const getChats = async (): Promise<ChatResponseDto[]> => {
  try {
    const response = await api.get("/chats");
    return response.data;
  } catch (error) {
    throw new Error("Error fetching chats");
  }
};

export const getChatMessages = async (
  id: string,
): Promise<MessageResponseDto[]> => {
  try {
    const response = await api.get(`/chats/${id}/messages`);
    return response.data;
  } catch (error) {
    throw new Error("Error fetching messages");
  }
};

export const createChat = async (
  currentUserId: string,
  otherUserId: string,
): Promise<ChatResponseDto> => {
  try {
    const response = await api.post("/chats", {
      user1_id: currentUserId,
      user2_id: otherUserId,
    });

    toastSuccess("Chat created successfully");
    return response.data;
  } catch (error) {
    throw new Error("Error creating chat");
  }
};
