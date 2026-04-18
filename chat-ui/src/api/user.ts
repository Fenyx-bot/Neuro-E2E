import { User } from "../types";
import api from "./api";

export const getUsers = async (): Promise<User[]> => {
  try {
    const response = await api.get("/users");
    return response.data;
  } catch (error) {
    throw new Error("Error fetching users");
  }
};
