export interface ErrorDetails {
  message: string;
  iconName: string;
}

export const ERROR_MAP: Record<number, ErrorDetails> = {
  400: {
    message: 'Sorry, the server cannot or will not process the request.',
    iconName: 'card-400'
  },
  404: {
    message: 'Sorry, the page you are looking for does not exist or has been moved.',
    iconName: 'card-404'
  },
  500: {
    message: 'Sorry, an unexpected condition was encountered.',
    iconName: 'card-500'
  }
};
