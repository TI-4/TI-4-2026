import { Panel } from '../../components/Panel';
import { PhotoFrame } from '../../components/PhotoFrame';
import { UserActionInfo } from '../../components/UserActionInfo';
import { IconText } from '../../components/IconText';
import { Tag } from '../../components/Tag';
import MapPinIcon from '../../assets/svg/icons/icon_location.svg?react';


export interface IncidentCardProps {
  location: string;
  status: string;
  statusColor?: 'blue' | 'green' | 'yellow' | 'red' | 'gray' | 'white';
  photoUrl?: string;
  title: string;
  reporterName: string;
  reportTime: string;
  reporterAvatar?: string;
  description: string;
  compact?: boolean;
  className?: string;
}

export const IncidentCard = ({
  location,
  status,
  statusColor = 'red',
  photoUrl,
  title,
  reporterName,
  reportTime,
  reporterAvatar,
  description,
  compact = false,
  className = ''
}: IncidentCardProps) => {
  return (
    <Panel color="white" outerClassName={`w-full ${compact ? 'max-w-full' : 'max-w-2xl'} ${className}`} innerClassName={`${compact ? 'p-4 gap-3' : 'p-6 gap-4'} flex flex-col`}>
      {/* Ubication and state */}
      <div className="flex justify-between items-center mb-1">
        <IconText icon={<MapPinIcon className={compact ? 'w-5 h-5' : 'w-8 h-8'} />} text={location} className={compact ? '!text-lg' : '!text-[26px]'} />
        <Tag label={status} color={statusColor as any} className={`${compact ? '!text-sm !px-3 !py-1' : '!text-lg !px-4 !py-1.5'} uppercase`} />
      </div>

      {/* Incident image */}
      <PhotoFrame src={photoUrl} className={`w-full ${compact ? 'h-32 rounded-xl' : 'h-72 rounded-3xl'} object-cover mb-1`} />

      {/* Incident name */}
      <h2 className={`${compact ? 'text-2xl' : 'text-[38px]'} font-medium text-gray-800 leading-tight`}>
        {title}
      </h2>

      {/* Report Information */}
      {!compact && (
        <UserActionInfo
          title={`Reportado por ${reporterName}`}
          subtitle={reportTime}
          avatarSrc={reporterAvatar}
          className={`!p-0 !bg-transparent origin-left scale-90`}
        />
      )}

      {/* Description */}
      <p className={`${compact ? 'text-base' : 'text-[22px]'} text-gray-700 leading-relaxed mt-1 line-clamp-2`}>
        "{description}"
      </p>
    </Panel>
  );
};
