import React, { useEffect, useState } from 'react';

interface ModalOverlayProps {
  isOpen: boolean;
  onClose: () => void;
  children: React.ReactNode;
}

export const ModalOverlay = ({ isOpen, onClose, children }: ModalOverlayProps) => {
  const [shouldRender, setShouldRender] = useState(isOpen);

  useEffect(() => {
    if (isOpen) {
      setShouldRender(true);
    } else {
      const timer = setTimeout(() => setShouldRender(false), 300);
      return () => clearTimeout(timer);
    }
  }, [isOpen]);

  if (!shouldRender) return null;

  return (
    <div 
      className={`fixed inset-0 z-[100] flex items-center justify-center backdrop-blur-sm transition-all duration-300 ${
        isOpen ? 'opacity-100 pointer-events-auto bg-black/40' : 'opacity-0 pointer-events-none bg-transparent'
      }`}
      onClick={onClose}
    >
      <div 
        className={`transition-all duration-300 transform ${
          isOpen ? 'scale-100 translate-y-0 opacity-100' : 'scale-95 translate-y-4 opacity-0'
        }`}
        onClick={(e) => e.stopPropagation()}
      >
        {children}
      </div>
    </div>
  );
};
