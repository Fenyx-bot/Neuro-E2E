import { Routes, Route, Navigate, BrowserRouter } from "react-router-dom";
import { Login } from "./pages/Login";
import { Signup } from "./pages/Signup";
import { AuthProvider } from "./context/AuthContext";
import { ProtectedRoute } from "./components/ProtectedRoute";
import Home from "./pages/Home";
import { Toaster } from "react-hot-toast";
import { Navbar } from "./components/Navbar";
import { ChatPage } from "./pages/ChatPage";
import NewChatPage from "./pages/NewChatPage";

function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <div>
          <Navbar />
          <Routes>
            <Route path="/login" element={<Login />} />
            <Route path="/signup" element={<Signup />} />
            <Route path="/" element={<Navigate to="/login" />} />
            <Route
              path="/new-chat"
              element={
                <ProtectedRoute>
                  <NewChatPage />
                </ProtectedRoute>
              }
            />
            <Route
              path="/chat"
              element={
                <ProtectedRoute>
                  <Home />
                </ProtectedRoute>
              }
            />
            <Route
              path="/chat/:chatId"
              element={
                <ProtectedRoute>
                  <ChatPage />
                </ProtectedRoute>
              }
            />

            <Route path="*" element={<Navigate to="/login" />} />
          </Routes>
          <Toaster position="top-right" />
        </div>
      </BrowserRouter>
    </AuthProvider>
  );
}

export default App;
