export interface Environment {
  production: boolean;
  apiUrl: string;
  identityUrl: string;
  storageUrl: string;
}

export const environment: Environment = {
  production: false,
  apiUrl: 'https://localhost:5110',
  identityUrl: 'http://localhost:8080',
  storageUrl: 'http://localhost:1433'
};
