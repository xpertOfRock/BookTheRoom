import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { isAuthorized } from "../../services/auth";
import { postComment } from "../../services/hotels";

function CreateCommentForm({ hotelId, hasRatedComments }) {
  const [commentText, setCommentText] = useState("");
  const [rating, setRating] = useState(0);
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const handleRatingClick = (value) => {
    if (!hasRatedComments) {
      setRating(value);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!commentText.trim()) {
      setError("Comment cannot be empty.");
      return;
    }

    if (!hasRatedComments && rating === 0) {
      setError("Please provide a rating.");
      return;
    }

    setError("");
    setLoading(true);

    try {
      await postComment(hotelId, {
        description: commentText,
        userScore: rating,
      });
      setCommentText("");
      setRating(0);
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

      {!hasRatedComments ? (
        <div className="mb-4">
          <label className="block text-gray-700 font-medium">Your Rating:</label>
          <div className="flex items-center space-x-2">
            {[1, 2, 3, 4, 5, 6, 7, 8, 9, 10].map((value) => (
              <button
                type="button"
                key={value}
                onClick={() => handleRatingClick(value)}
                className={`text-xl ${
                  value <= rating ? "text-yellow-400" : "text-gray-300"
                } hover:text-yellow-500`}
              >
                ★
              </button>
            ))}
          </div>
        </div>
      ) : (
        <p className="text-gray-500 text-sm">You have already rated this hotel</p>
      )}

      {error && <p className="text-red-500 mb-2">{error}</p>}
      
      <div className="flex justify-end">
        {isAuthorized() ? (
          <button
            type="submit"
            className="bg-indigo-600 text-white px-4 py-2 rounded-lg shadow-md hover:bg-indigo-700 hover:text-white transition-all disabled:opacity-50"
            disabled={loading}
          >
            {loading ? "Posting..." : "Post Comment"}
          </button>
        ) : (
          <p className="text-gray-700">
            You need to be logged in to leave a comment.{" "}
            <span
              onClick={() => navigate("/login")}
              className="text-blue-500 cursor-pointer underline"
            >
              Log in here
            </span>
          </p>
        )}
      </div>
    </form>
  );
}

export default CreateCommentForm;