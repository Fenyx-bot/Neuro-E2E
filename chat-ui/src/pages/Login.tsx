import React, { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { LoginData } from "../types";
import { AuthLayout } from "../components/AuthLayout";
import { Input } from "../components/Input";
import { Button } from "../components/Button";
import { authLogin } from "../api/auth";
import { useAuth } from "../context/AuthContext";
import toast from "react-hot-toast";

export const Login: React.FC = () => {
  const [formData, setFormData] = useState<LoginData>({
    email: "",
    password: "",
  });
  const { login } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      const response = await authLogin(formData);
      login(response);
      toast.success("Logged in successfully");
      navigate("/chat");
    } catch (error) {
      toast.error("Error logging in");
    }
  };

  return (
    <AuthLayout
      title="Welcome back"
      subtitle="Log in to your account to continue"
    >
      <form onSubmit={handleSubmit} className="space-y-4">
        <Input
          label="Email"
          type="email"
          required
          value={formData.email}
          onChange={(e) => setFormData({ ...formData, email: e.target.value })}
          placeholder="you@example.com"
        />
        <Input
          label="Password"
          type="password"
          required
          value={formData.password}
          onChange={(e) =>
            setFormData({ ...formData, password: e.target.value })
          }
          placeholder="••••••••"
        />

        <div className="flex items-center justify-end">
          <Link
            to="/forgot-password"
            className="text-sm text-indigo-600 hover:text-indigo-500"
          >
            Forgot your password?
          </Link>
        </div>

        <Button type="submit">Log in</Button>

        <p className="text-center text-sm text-gray-600 mt-4">
          Don't have an account?{" "}
          <Link
            to="/signup"
            className="text-indigo-600 hover:text-indigo-500 font-medium"
          >
            Sign up
          </Link>
        </p>
      </form>
    </AuthLayout>
  );
};
