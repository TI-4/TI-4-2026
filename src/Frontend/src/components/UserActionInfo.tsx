import { Avatar } from './Avatar';

interface UserActionInfoProps {
  title: string;
  subtitle: string;
  avatarSrc?: string | null;
  className?: string;
}

export const UserActionInfo = ({ title, subtitle, avatarSrc, className = '' }: UserActionInfoProps) => {
  return (
    <div className={`flex items-center gap-4 bg-white p-2 ${className}`}>
      <Avatar src={avatarSrc} className="w-[72px] h-[72px]" />
      <div className="flex flex-col justify-center">
        <h3 className="text-[28px] leading-tight font-medium text-page-dark">{title}</h3>
        <p className="text-[22px] leading-tight font-medium text-gray-400 mt-1">{subtitle}</p>
      </div>
    </div>
  );
};
