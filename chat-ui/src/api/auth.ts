import { User, LoginData, SignupData } from "../types";
import api from "./api";

export const authSignup = async (signupData: SignupData): Promise<void> => {
  try {
    const response = await api.post("/auth/register", signupData);

    return response.data;
  } catch (error) {
    throw new Error("Error signing up");
  }
};

export const authLogin = async (loginData: LoginData): Promise<string> => {
  try {
    const response = await api.post("/auth/login", loginData);
    return response.data;
  } catch (error) {
    throw new Error("Error logging in");
  }
};

export const authCurrentUser = async (): Promise<User> => {
  try {
    const response = await api.get("/users/current");
    return response.data;
  } catch (error) {
    throw new Error("Error fetching current user");
  }
};
