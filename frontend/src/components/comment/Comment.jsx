import React from "react";

function Comment({ username, description, createdAt, isCurrentUser }) {
  return (
    <div
      className={`p-4 rounded-lg ${
        isCurrentUser ? "bg-blue-100" : "bg-purple-100"
      } shadow-md`}
    >
      <div className="flex justify-between items-center mb-2">
        <h4 className="font-bold text-gray-800">{username}</h4>
        <span className="text-sm text-gray-500">{new Date(createdAt).toLocaleDateString()}</span>
      </div>
      <p className="text-gray-700">{description}</p>
    </div>
  );
}

export default Comment;
