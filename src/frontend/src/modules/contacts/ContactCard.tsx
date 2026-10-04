import { Panel } from '../../components/layout/Panel';
import { PhotoFrame } from '../../components/media/PhotoFrame';
import { Button } from '../../components/ui/Button';
import EyeIcon from '../../assets/svg/icons/icon_eye.svg?react';

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

      {/* Variants */}
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
              <EyeIcon className="w-5 h-5" />
              INFO
            </span>
          </Button>
        </div>
      </div>
    </Panel>
  );
};
