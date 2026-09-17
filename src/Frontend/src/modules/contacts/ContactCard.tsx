import React from 'react';
import { Panel } from '../../components/Panel';
import { PhotoFrame } from '../../components/PhotoFrame';
import { Button } from '../../components/Button';

const EyeIcon = () => (
  <svg className="w-5 h-5" fill="currentColor" viewBox="0 0 24 24">
    <path d="M12 4.5C7 4.5 2.73 7.61 1 12c1.73 4.39 6 7.5 11 7.5s9.27-3.11 11-7.5c-1.73-4.39-6-7.5-11-7.5zM12 17c-2.76 0-5-2.24-5-5s2.24-5 5-5 5 2.24 5 5-2.24 5-5 5zm0-8c-1.66 0-3 1.34-3 3s1.34 3 3 3 3-1.34 3-3-1.34-3-3-3z" />
  </svg>
);

export interface ContactCardProps {
  name: string;
  role: string;
  department: string;
  photoUrl?: string;
  onInfoClick?: () => void;
  className?: string;
}

export const ContactCard = ({
  name,
  role,
  department,
  photoUrl,
  onInfoClick,
  className = '',
}: ContactCardProps) => {
  return (
    <Panel color="white" outerClassName={`w-full max-w-2xl ${className}`} innerClassName="p-6 flex flex-row gap-6">
      {/* Photo */}
      <PhotoFrame src={photoUrl} className="w-40 h-40 rounded-2xl" />

      {/* Main */}
      <div className="flex flex-col flex-1 justify-between">
        <div className="flex flex-col pt-2">
          <h3 className="text-3xl font-medium text-gray-800 mb-1 uppercase leading-tight">
            {name}
          </h3>
          <p className="text-xl text-gray-700 leading-snug">{role}</p>
          <p className="text-xl text-gray-700 leading-snug">{department}</p>
        </div>

        {/* Button */}
        <div className="flex justify-end">
          <Button
            variant="solid"
            onClick={onInfoClick}
            className="w-max px-6 py-2"
            size="sm"
          >
            <span className="flex items-center justify-center gap-2 text-lg font-medium">
              <EyeIcon />
              INFO
            </span>
          </Button>
        </div>
      </div>
    </Panel>
  );
};
