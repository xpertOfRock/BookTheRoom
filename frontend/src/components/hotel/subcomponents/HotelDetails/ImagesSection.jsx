import React from "react";

function ImagesSection({ images, onImageClick }) {
  if (!images || images.length === 0) return null;

  return (
    <div className="px-6">
      <h3 className="text-2xl font-semibold mt-6 text-gray-700">More Images</h3>
      <div className="bg-white grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4 my-4 p-2">
        {images.map((image, index) => (
          <div
            key={index}
            className="relative group cursor-pointer"
            onClick={() => onImageClick(index)}
          >
            <img
              className="rounded-lg shadow-md w-full h-[200px] object-cover transition-transform duration-300 group-hover:scale-105"
              src={image}
              alt={`Image ${index + 1}`}
            />
          </div>
        ))}
      </div>
    </div>
  );
}

export default ImagesSection;