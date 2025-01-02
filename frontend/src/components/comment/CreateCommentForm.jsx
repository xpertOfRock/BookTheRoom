import React, { useState } from "react";
import { postComment } from "../../services/hotels";

function CreateCommentForm({ hotelId }) {
  const [commentText, setCommentText] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!commentText.trim()) {
      setError("Comment cannot be empty.");
      return;
    }

    setError("");
    setLoading(true);

    try {
      console.log(commentText);
      const newComment = await postComment(hotelId, commentText);
      setCommentText("");
      window.location.reload();
    } catch (err) {
      console.error("Error posting comment:", err);
      setError("Failed to post comment. Please try again.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <form onSubmit={handleSubmit} className="mt-4 bg-indigo-100 p-4 rounded-lg shadow-md">
      <div className="mb-4">
        <label htmlFor="comment" className="block text-gray-700 font-medium">
          Your Comment:
        </label>
        <textarea
          id="comment"
          rows="4"
          value={commentText}
          onChange={(e) => setCommentText(e.target.value)}
          className="w-full border border-gray-300 rounded-lg p-2 mt-1 focus:outline-none focus:ring-2 focus:ring-blue-500"
          placeholder="Write your comment here..."
        ></textarea>
      </div>
      {error && <p className="text-red-500 mb-2">{error}</p>}
      <div className="flex justify-end">
        <button
          type="submit"
          className="bg-blue-500 text-white px-4 py-2 rounded-lg shadow-md hover:bg-blue-600 transition-all disabled:opacity-50"
          disabled={loading}
        >
          {loading ? "Posting..." : "Post Comment"}
        </button>
      </div>
    </form>
  );
}

export default CreateCommentForm;
