import { type PageColor, bgPageColors, borderPageColors } from '../constants/colors';

interface MapMarkerProps extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  icon?: React.ReactNode;
  color?: PageColor;
}

export const MapMarker = ({ icon, color = 'blue', className = '', ...props }: MapMarkerProps) => {

  return (
    <button
      type="button"
      className={`relative group w-14 h-14 rounded-full flex items-center justify-center text-white shadow-lg transition-transform duration-300 hover:-translate-y-2 hover:shadow-xl focus:outline-none rounded-br-none rotate-45 border-[3px] border-white ${bgPageColors[color]} ${className}`}
      {...props}
    >
      {/* Anillo de selección personalizado (aparece de abajo hacia arriba) */}
      <div className="absolute w-[100px] h-[100px] top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 -rotate-45 pointer-events-none flex items-center justify-center [clip-path:inset(100%_0_0_0)] group-focus:[clip-path:inset(0_0_0_0)] transition-[clip-path] duration-300 ease-out z-[-1]">
        {/* Forma del anillo, que se vuelve a rotar 45 grados para encajar con el marcador. 68px = 56px (w-14) + 12px de gap (6px por lado) */}
        <div className={`w-[68px] h-[68px] rotate-45 rounded-full rounded-br-none border-[3px] ${borderPageColors[color]} opacity-80`} />
      </div>

      {/* Círculo blanco interno */}
      <div className="w-10 h-10 bg-white rounded-full flex items-center justify-center -rotate-45 shadow-inner z-10">
        <span className="text-page-dark flex items-center justify-center w-6 h-6">
          {icon}
        </span>
      </div>
    </button>
  );
};
