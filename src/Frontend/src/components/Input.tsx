import React from 'react';

type InputProps = {
  label?: string;
  error?: string;
  multiline?: boolean;
  icon?: React.ReactNode;
} & React.InputHTMLAttributes<HTMLInputElement> & React.TextareaHTMLAttributes<HTMLTextAreaElement>;

export const Input = React.forwardRef<any, InputProps>(
  ({ label, error, className = '', multiline, icon, ...props }, ref) => {
    return (
      <div className="flex flex-col gap-1 w-full mt-5">
        <div className="relative w-full">

          {icon && (
            <div className="absolute left-4 top-1/2 -translate-y-1/2 z-20 text-gray-700 flex items-center justify-center pointer-events-none">
              {icon}
            </div>
          )}

          {multiline ? (
            <textarea
              ref={ref}
              placeholder={props.placeholder || " "}
              className={`peer w-full ${icon ? 'pl-12 pr-4' : 'px-4'} py-3 border-2 rounded-[18px] outline-none bg-white relative z-10 transition-colors
                placeholder-transparent focus:placeholder-gray-400
                ${error ? 'border-red-300' : 'border-gray-400'}
                ${className}
              `}
              {...(props as any)}
            />
          ) : (
            <input
              ref={ref}
              placeholder={props.placeholder || " "}
              className={`peer w-full ${icon ? 'pl-12 pr-4' : 'px-4'} py-3 border-2 rounded-[18px] outline-none bg-white relative z-10 transition-colors
                placeholder-transparent focus:placeholder-gray-400
                ${error ? 'border-red-300' : 'border-gray-400'}
                ${className}
              `}
              {...(props as any)}
            />
          )}

          {/* FLOATING LABEL */}
          {label && (
            <label
              className={`absolute ${icon ? 'left-12' : 'left-4'} top-3 transition-all duration-300 pointer-events-none origin-left z-30
                peer-placeholder-shown:translate-y-0 peer-placeholder-shown:scale-110 peer-placeholder-shown:text-gray-500 peer-placeholder-shown:font-normal
                peer-focus:-translate-y-10 peer-focus:scale-100 peer-focus:font-bold
                -translate-y-10 scale-100 font-bold
                ${error ? 'text-page-red peer-focus:text-page-red' : 'text-gray-700 peer-focus:text-page-blue'}
              `}
            >
              {label}
            </label>
          )}

          <div
            className={`absolute inset-0 rounded-[18px] pointer-events-none p-[4px] transition-[clip-path] duration-300 ease-out z-20
              ${error
                ? 'animate-error-pulse [clip-path:inset(-10px_-10px_-10px_-10px)]'
                : 'bg-[linear-gradient(to_right,var(--color-page-blue)_50%,var(--color-page-yellow)_50%)] [clip-path:inset(-10px_100%_-10px_0)] peer-focus:[clip-path:inset(-10px_-10px_-10px_-10px)]'
              }
            `}
            style={{
              WebkitMask: 'linear-gradient(#fff 0 0) content-box, linear-gradient(#fff 0 0)',
              WebkitMaskComposite: 'xor',
              maskComposite: 'exclude'
            }}
          />
        </div>

        {error && <span className="text-xs text-red-500 pl-4">{error}</span>}
      </div>
    );
  }
);

Input.displayName = 'Input';
