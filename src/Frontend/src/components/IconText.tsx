
interface IconTextProps {
  icon: React.ReactNode;
  text: string;
  className?: string;
  color?: string;
}

export const IconText = ({ icon, text, className = '', color }: IconTextProps) => {
  return (
    <div 
      className={`flex items-center gap-3 ${className} ${!color ? 'text-page-dark' : ''}`}
      style={color ? { color } : undefined}
    >
      <span className="flex items-center justify-center text-3xl">
        {icon}
      </span>
      <span className="font-medium text-xl">{text}</span>
    </div>
  );
};
