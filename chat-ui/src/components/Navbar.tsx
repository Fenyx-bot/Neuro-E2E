import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import {
  MdChatBubble,
  MdLogout,
  MdSettings,
  MdVerifiedUser,
} from "react-icons/md";
import { FaMessage } from "react-icons/fa6";

export const Navbar = () => {
  const { isAuthenticated, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate("/login");
  };

  return (
    <nav className="bg-white shadow-lg">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex justify-between h-16">
          <div className="flex items-center">
            <Link to="/chat" className="flex items-center space-x-2">
              <MdChatBubble className="h-8 w-8 text-indigo-600" />
              <span className="text-xl font-bold text-gray-900">ChatHub</span>
            </Link>
          </div>

          <div className="flex items-center space-x-4">
            {isAuthenticated ? (
              <>
                <Link
                  to="/chat"
                  className="text-gray-700 hover:text-indigo-600"
                >
                  <FaMessage className="h-6 w-6" />
                </Link>
                <button
                  onClick={handleLogout}
                  className="flex items-center space-x-1 text-gray-700 hover:text-indigo-600"
                >
                  <MdLogout className="h-5 w-5" />
                  <span>Logout</span>
                </button>
              </>
            ) : (
              <Link
                to="/login"
                className="flex items-center space-x-1 text-gray-700 hover:text-indigo-600"
              >
                <MdVerifiedUser className="h-5 w-5" />
                <span>Login</span>
              </Link>
            )}
          </div>
        </div>
      </div>
    </nav>
  );
};
