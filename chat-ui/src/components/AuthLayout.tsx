import React from "react";
import { AuthLayoutProps } from "../types";
import { MdFingerprint } from "react-icons/md";

export const AuthLayout: React.FC<AuthLayoutProps> = ({
  children,
  title,
  subtitle,
}) => {
  return (
    <div className="min-h-screen bg-gradient-to-br from-indigo-100 via-purple-50 to-pink-100 flex items-center justify-center p-4">
      <div className="max-w-md w-full bg-white rounded-2xl shadow-xl p-8">
        <div className="flex flex-col items-center mb-8">
          <div className="bg-indigo-600 p-3 rounded-full mb-4">
            <MdFingerprint className="w-8 h-8 text-white" />
          </div>
          <h1 className="text-2xl font-bold text-gray-900">{title}</h1>
          <p className="text-gray-600 text-center mt-2">{subtitle}</p>
        </div>
        {children}
      </div>
    </div>
  );
};
