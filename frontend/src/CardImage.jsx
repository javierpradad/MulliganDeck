import { useState } from "react";

function CardImage({ card, onHover }) {
  const [faceIndex, setFaceIndex] = useState(0);

  const hasBack = card.faces && card.faces.length > 1;

  const currentImage = hasBack
    ? card.faces[faceIndex]?.imageUri
    : card.imageUri;

  const flip = (e) => {
    e.stopPropagation();
    setFaceIndex((i) => (i === 0 ? 1 : 0));
  };

  return (
    <div
      className="relative group"
      onMouseEnter={() => onHover && onHover(currentImage)}
    >
      {currentImage ? (
        <img
          src={currentImage}
          alt={card.name}
          loading="lazy"
          className="w-full rounded-xl shadow-lg"
        />
      ) : (
        <div className="w-full aspect-[5/7] bg-neptune-900 border border-neptune-800 rounded-xl flex items-center justify-center p-3">
          <span className="text-neptune-400 text-sm text-center">{card.name}</span>
        </div>
      )}

      {hasBack && (
        <button
          onClick={flip}
          className="absolute top-2 right-2 bg-neptune-950/80 text-neptune-100 rounded-full w-8 h-8 flex items-center justify-center hover:bg-neptune-800 transition opacity-0 group-hover:opacity-100"
          title="Girar carta"
        >
          ⟳
        </button>
      )}
    </div>
  );
}

export default CardImage;