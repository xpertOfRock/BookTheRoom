import React from "react";
import CreateCommentForm from "../../../comment/CreateCommentForm";
import Comment from "../../../comment/Comment";

function CommentsSection({ hotel, hasRatedComments, currentUserId, onAddComment }) {
  return (
    <div className="w-full p-4 rounded-lg shadow-md border-[3px] border-indigo-300">
      <h3 className="text-2xl font-semibold text-gray-700 mb-2">Comments</h3>
      
      <CreateCommentForm 
        hotelId={hotel.id}
        hasRatedComments={hasRatedComments}
        onAddComment={onAddComment}
        />

      {hotel.comments && hotel.comments.length > 0 ? (
        <div className="border-2 border-indigo-300 mt-6 p-6 rounded-lg shadow-lg">
          <div className="space-y-4">
            {hotel.comments.map((comment) => (
              <Comment
                key={comment.id}
                username={comment.username}
                description={comment.description}
                createdAt={comment.createdAt}
                isCurrentUser={currentUserId === comment.userId}
                userScore={comment.userScore}
              />
            ))}
          </div>
        </div>
      ) : (
        <p className="text-gray-500">No comments yet. Be the first to leave a comment!</p>
      )}
    </div>
  );
}

export default CommentsSection;