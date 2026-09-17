import React, { useState, useEffect } from 'react';

interface LoadingSpinnerProps {
  size?: 'sm' | 'md' | 'lg' | 'xl';
  color?: string;
  className?: string;
  text?: string;
  texts?: string[];
}

export const LoadingSpinner = ({ 
  size = 'md', 
  color = 'text-page-blue', 
  className = '',
  text,
  texts
}: LoadingSpinnerProps) => {
  const sizeClasses = {
    sm: 'w-8 h-8',
    md: 'w-12 h-12',
    lg: 'w-20 h-20',
    xl: 'w-32 h-32'
  };

  const hasMultipleTexts = Array.isArray(texts) && texts.length > 0;
  const [currentText, setCurrentText] = useState(() => hasMultipleTexts ? texts[0] : text);
  const [isFading, setIsFading] = useState(false);

  useEffect(() => {
    if (!hasMultipleTexts || !texts) return;

    // Cambiar de texto cada 3.5 segundos en total (más tiempo visible)
    const interval = setInterval(() => {
      // Inicia el desvanecimiento y movimiento hacia abajo
      setIsFading(true);
      
      // Una vez desvanecido (700ms), cambiar el texto y volver a aparecer hacia arriba
      setTimeout(() => {
        setCurrentText((prevText) => {
          let nextIndex;
          do {
            nextIndex = Math.floor(Math.random() * texts.length);
          } while (texts[nextIndex] === prevText && texts.length > 1);
          return texts[nextIndex];
        });
        setIsFading(false);
      }, 700);

    }, 3500); 

    return () => clearInterval(interval);
  }, [JSON.stringify(texts), hasMultipleTexts]);

  return (
    <div className={`flex flex-col items-center justify-center gap-4 ${className}`}>
      <div className={`relative flex items-center justify-center ${sizeClasses[size]} ${color}`}>
        {/* Base giratoria lenta constante */}
        <div className="absolute inset-0 w-full h-full animate-[spin_4s_linear_infinite]">
          {/* Anillo que recibe el "boost" de aceleración (2 líneas: azul y amarilla) */}
          <svg
            className="w-full h-full animate-spin-expo"
            xmlns="http://www.w3.org/2000/svg"
            fill="none"
            viewBox="0 0 24 24"
          >
            {/* Arco 1 (Azul / Current Color) */}
            <circle cx="12" cy="12" r="11" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" style={{ strokeDasharray: '18 52' }} className="opacity-80" />
            {/* Arco 2 (Amarillo) */}
            <circle cx="12" cy="12" r="11" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" style={{ strokeDasharray: '18 52', strokeDashoffset: '34.5' }} className="text-page-yellow opacity-80" />
          </svg>
        </div>

        {/* Símbolo UCT Animado (Centro) - Cruz y Araucaria */}
        <svg
          className="absolute inset-0 w-full h-full p-0.5 text-gray-400"
          xmlns="http://www.w3.org/2000/svg"
          fill="none"
          viewBox="0 0 24 24"
          stroke="currentColor"
          strokeWidth="2.5"
          strokeLinecap="round"
        >
          <defs>
            <clipPath id="uct-tree-clip">
              <circle cx="12" cy="12" r="10.8" />
            </clipPath>
          </defs>
          <g clipPath="url(#uct-tree-clip)" style={{ animation: 'uct-group-fade 2.8s infinite cubic-bezier(0.16, 1, 0.3, 1)' }}>
            {/* Tronco vertical (centrado) */}
            <path 
              d="M 12 22.8 L 12 1.2" 
              style={{ 
                strokeDasharray: 21.6, 
                animation: 'uct-draw-line 2.8s infinite cubic-bezier(0.16, 1, 0.3, 1)' 
              }} 
            />
            {/* Brazo horizontal derecho (expandido 3/4 más de su tamaño: de x=12 a x=20.7, sin esquinas redondeadas) */}
            <path 
              d="M 12 9.5 L 20.7 9.5" 
              strokeLinecap="butt"
              style={{ '--len': '8.7', strokeDasharray: '8.7', animation: 'uct-sprout-2 2.8s infinite cubic-bezier(0.16, 1, 0.3, 1)' } as React.CSSProperties} 
            />
            {/* Ramas curvas izquierda (3 arcos paralelos con distancia exactamente constante de 4.0 unidades en cualquier ángulo) */}
            {/* 1. Más chica (Raíz y=5.5 -> Radio 4.3 -> Punta en x=7.7, y=1.2) */}
            <path 
              d="M 12 5.5 A 4.3 4.3 0 0 1 7.7 1.2" 
              style={{ '--len': '6.8', strokeDasharray: '6.8', animation: 'uct-sprout-1 2.8s infinite cubic-bezier(0.16, 1, 0.3, 1)' } as React.CSSProperties} 
            />
            {/* 2. Mediana (Raíz y=9.5 -> Radio 8.3 -> Punta en x=3.7, y=1.2, comparte raíz y=9.5 con el brazo derecho) */}
            <path 
              d="M 12 9.5 A 8.3 8.3 0 0 1 3.7 1.2" 
              style={{ '--len': '13.0', strokeDasharray: '13.0', animation: 'uct-sprout-2 2.8s infinite cubic-bezier(0.16, 1, 0.3, 1)' } as React.CSSProperties} 
            />
            {/* 3. Más grande (Raíz y=13.5 -> Radio 12.3 -> Recortada limpiamente en el borde circular) */}
            <path 
              d="M 12 13.5 A 12.3 12.3 0 0 1 -0.3 1.2" 
              style={{ '--len': '19.3', strokeDasharray: '19.3', animation: 'uct-sprout-3 2.8s infinite cubic-bezier(0.16, 1, 0.3, 1)' } as React.CSSProperties} 
            />
          </g>
        </svg>
      </div>
      
      {/* Texto debajo del spinner */}
      {(currentText || text) && (
        <p 
          className={`text-gray-500 font-medium transition-all duration-700 ease-in-out text-center max-w-[250px] leading-snug ${isFading ? 'opacity-0 translate-y-3' : 'opacity-100 translate-y-0'} ${!hasMultipleTexts ? 'animate-pulse' : ''}`}
        >
          {hasMultipleTexts ? currentText : text}
        </p>
      )}
    </div>
  );
};
