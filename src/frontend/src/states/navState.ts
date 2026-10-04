import { create } from 'zustand';

interface NavState {
  isExpanded: boolean;
  setIsExpanded: (expanded: boolean) => void;
}

export const useNavStore = create<NavState>((set) => ({
  isExpanded: false,
  setIsExpanded: (expanded) => set({ isExpanded: expanded }),
}));
