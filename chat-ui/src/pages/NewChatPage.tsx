import { useEffect, useState } from "react";
import { User } from "../types";
import { useNavigate } from "react-router-dom";
import { LuMessageSquarePlus } from "react-icons/lu";
import { FiSearch, FiUsers } from "react-icons/fi";
import { getUsers } from "../api/user";
import { useAuth } from "../context/AuthContext";
import { createChat } from "../api/chat";

export const NewChatPage = () => {
  const navigate = useNavigate();
  const [searchQuery, setSearchQuery] = useState("");
  const [users, setUsers] = useState<User[]>([]);
  const { user: currentUser } = useAuth();

  useEffect(() => {
    const fetchUsers = async () => {
      const response = await getUsers();

      // Remove the current user from the list
      const filteredUsers = response.filter(
        (user) => user.id !== currentUser?.id,
      );

      setUsers(filteredUsers);
    };

    fetchUsers();
  }, []);

  const filteredUsers = users.filter((user) =>
    user.display_name.toLowerCase().includes(searchQuery.toLowerCase()),
  );

  const handleUserSelect = async (user: User) => {
    try {
      if (user.id === currentUser?.id) {
        throw new Error("You cannot chat with yourself");
      }

      const response = await createChat(currentUser!.id, user.id);
      navigate(`/chat/${response.id}`);
    } catch (error) {
      console.error("Error creating chat:", error);
    }
  };

  return (
    <div className="container mx-auto max-w-4xl px-4 py-6">
      <div className="bg-white rounded-lg shadow-lg">
        {/* Header */}
        <div className="px-6 py-4 border-b border-gray-200 flex items-center space-x-2">
          <LuMessageSquarePlus className="h-6 w-6 text-indigo-600" />
          <h1 className="text-2xl font-semibold text-gray-800">New Chat</h1>
        </div>

        {/* Search Bar */}
        <div className="p-4 border-b border-gray-200">
          <div className="relative">
            <FiSearch className="h-5 w-5 absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400" />
            <input
              type="text"
              placeholder="Search users..."
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-transparent outline-none"
            />
          </div>
        </div>

        {/* User List */}
        <div className="divide-y divide-gray-200">
          {filteredUsers.length === 0 ? (
            <div className="p-8 text-center text-gray-500">
              <FiUsers className="h-12 w-12 mx-auto mb-4 text-gray-400" />
              <p className="text-lg font-medium">No users found</p>
              <p className="mt-1">Try adjusting your search</p>
            </div>
          ) : (
            filteredUsers.map((user) => (
              <div
                key={user.id}
                onClick={() => handleUserSelect(user)}
                className="p-4 hover:bg-gray-50 cursor-pointer transition-colors flex items-center space-x-4"
              >
                <img
                  src={user.profile_picture}
                  alt={user.display_name}
                  className="h-10 w-10 rounded-full"
                />
                <div className="flex-1">
                  <h2 className="text-lg font-medium text-gray-900">
                    {user.display_name}
                  </h2>
                  <p className="text-sm text-gray-500">
                    Joined {new Date(user.created_at).toLocaleDateString()}
                  </p>
                </div>
                <LuMessageSquarePlus className="h-5 w-5 text-gray-400 hover:text-indigo-600" />
              </div>
            ))
          )}
        </div>
      </div>
    </div>
  );
};

export default NewChatPage;
