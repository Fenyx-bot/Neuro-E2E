export interface ChatResponseDto {
  id: string;
  name: string;
  user1_id: string;
  user2_id: string;
  user1_name: string;
  user2_name: string;
  last_message_at: string;
}

export interface MessageResponseDto {
  id: string;
  sender_id: string;
  sender_username: string;
  encrypted_content: string;
  timestamp: string;
}

export interface SignupData {
  email: string;
  username: string;
  password: string;
  display_name: string;
}

export interface LoginData {
  email: string;
  password: string;
}

export interface AuthLayoutProps {
  children: React.ReactNode;
  title: string;
  subtitle: string;
}

export interface User {
  id: string;
  display_name: string;
  profile_picture: string;
  created_at: string;
}
