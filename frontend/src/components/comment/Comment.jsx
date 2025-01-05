import React from "react";

function Comment({ username, description, createdAt, isCurrentUser, userScore }) {
  return (
    <div
      className={`p-4 rounded-lg ${
        isCurrentUser ? "bg-indigo-100" : "bg-white border-2 border-indigo-300"
      } shadow-md`}
    >
      <div className="flex justify-between items-center mb-2">
        <h4 className="font-bold text-gray-800">
          {username}
          {userScore !== undefined && userScore !== null && userScore > 1 && (
            <span className="text-yellow-500 ml-2">{`${userScore.toFixed(1)} ★`}</span>
          )}
        </h4>
        <span className="text-sm text-gray-500">
          {new Date(createdAt).toLocaleString("en-US", {
            year: "numeric",
            month: "long",
            day: "numeric",
            hour: "2-digit",
            minute: "2-digit",
          })}
        </span>
      </div>
      <p className="text-gray-700">{description}</p>
    </div>
  );
}

export default Comment;