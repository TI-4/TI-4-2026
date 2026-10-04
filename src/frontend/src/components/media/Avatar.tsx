import { useState } from 'react';

interface AvatarProps {
  src?: string | null;
  alt?: string;
  className?: string;
}

export const Avatar = ({ src, alt = 'User Avatar', className = 'w-20 h-20' }: AvatarProps) => {
  const [hasError, setHasError] = useState(false);

  return (
    <div className={`rounded-full p-1.5 bg-gradient-to-b from-page-yellow from-50% to-page-blue to-50% shadow-md flex-shrink-0 ${className}`}>
      <div className="w-full h-full rounded-full overflow-hidden bg-page-gray-light flex items-center justify-center">
        {src && !hasError ? (
          <img
            src={src} alt={alt}
            className="w-full h-full object-cover"
            onError={() => setHasError(true)}
          />
        ) : (
          <span className="text-gray-400 text-xs font-semibold">N/A</span>
        )}
      </div>
    </div>
  );
};
