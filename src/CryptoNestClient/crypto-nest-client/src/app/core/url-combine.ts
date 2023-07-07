export const combineUrl = (baseUrl: string, url: string) => new URL(url, baseUrl).toString();
