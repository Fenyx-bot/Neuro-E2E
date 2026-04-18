import React, { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { SignupData } from "../types";
import { AuthLayout } from "../components/AuthLayout";
import { Input } from "../components/Input";
import { Button } from "../components/Button";
import { authSignup } from "../api/auth";

export const Signup: React.FC = () => {
  const [formData, setFormData] = useState<SignupData>({
    email: "",
    username: "",
    password: "",
    display_name: "",
  });
  const navigate = useNavigate();

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    authSignup(formData);
    navigate("/login");
  };

  return (
    <AuthLayout
      title="Create your account"
      subtitle="Join our community and start sharing your experiences"
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
          label="Username"
          required
          value={formData.username}
          onChange={(e) =>
            setFormData({ ...formData, username: e.target.value })
          }
          placeholder="johndoe"
        />
        <Input
          label="Display Name"
          required
          value={formData.display_name}
          onChange={(e) =>
            setFormData({ ...formData, display_name: e.target.value })
          }
          placeholder="John Doe"
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

        <Button type="submit">Sign up</Button>

        <p className="text-center text-sm text-gray-600 mt-4">
          Already have an account?{" "}
          <Link
            to="/login"
            className="text-indigo-600 hover:text-indigo-500 font-medium"
          >
            Log in
          </Link>
        </p>
      </form>
    </AuthLayout>
  );
};
