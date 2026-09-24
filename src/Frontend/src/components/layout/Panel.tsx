import { type PageColor, bgPageColors } from '../../constants/colors';
import { CloseButton } from '../ui/ExitButton'; // <-- Asegúrate de que la ruta coincida con tu archivo

interface PanelProps {
  children: React.ReactNode;
  color?: PageColor;
  withUctBorder?: boolean;
  outerClassName?: string;
  innerClassName?: string;
  onClose?: () => void;
}

export const Panel = ({
  children,
  color = 'white',
  withUctBorder = false,
  outerClassName = '',
  innerClassName = 'p-6 flex flex-col relative',
  onClose
}: PanelProps) => {

  const bgColorClass = color === 'gray'
    ? "bg-page-gray-light border border-gray-200"
    : bgPageColors[color];

  const renderCloseButton = () => {
      if (!onClose) return null;
      return (
        <div className="flex justify-end w-full mb-2">
          <CloseButton
            variant="ghost"
            size="lg"
            onClick={onClose}
          />
        </div>
      );
    };
  if (withUctBorder) {
    return (
      <div
        className={`bg-gradient-to-b from-page-blue from-50% to-page-yellow to-50% p-1.5 shadow-md w-max h-max rounded-3xl ${outerClassName}`}
      >
        <div className={`${bgColorClass} w-full h-full rounded-2xl ${innerClassName}`}>
          {renderCloseButton()}
          {children}
        </div>
      </div>
    );
  }

  return (
    <div className={`${bgColorClass} shadow-sm w-max h-max relative rounded-3xl ${outerClassName} ${innerClassName}`}>
      {renderCloseButton()}
      {children}
    </div>
  );
};
