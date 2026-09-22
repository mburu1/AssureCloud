export interface Environment {
  production: boolean;
  apiUrl: string;
  identityUrl: string;
  storageUrl: string;
}

export const environment: Environment = {
  production: true,
  apiUrl: 'https://api.assurecloud.example.com',
  identityUrl: 'https://login.assurecloud.example.com',
  storageUrl: 'https://storage.assurecloud.example.com'
};
