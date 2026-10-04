import { useState } from 'react';

interface PhotoFrameProps {
  src?: string | null;
  alt?: string;
  className?: string;
}

export const PhotoFrame = ({
  src,
  alt = 'Foto',
  className = 'w-48 h-48',
}: PhotoFrameProps) => {
  const [hasError, setHasError] = useState(false);

  return (
    <div className={`relative overflow-hidden rounded-2xl bg-page-gray-light flex items-center justify-center flex-shrink-0 ${className}`}>
      {src && !hasError ? (
        <img
          src={src}
          alt={alt}
          className="w-full h-full object-cover transition-all duration-300"
          onError={() => setHasError(true)}
        />
      ) : (
        <svg
          xmlns="http://www.w3.org/2000/svg"
          viewBox="0 0 24 24"
          fill="currentColor"
          className="w-1/2 h-1/2 text-page-gray-medium"
        >
          <path d="M21 19V5c0-1.1-.9-2-2-2H5c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h14c1.1 0 2-.9 2-2zM8.5 13.5l2.5 3.01L14.5 12l4.5 6H5l3.5-4.5z" />
        </svg>
      )}
    </div>
  );
};
