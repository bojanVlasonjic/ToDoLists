// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  baseUrl: 'http://localhost:50138/api/to-do-list',
  domainUrl: 'http://localhost:4200',
  auth: {
    domain: "dev-4h0-69-l.eu.auth0.com",
    clientID: "jeBhIoEIOLSbqNUqVH95zTU8I2GwbXHP",
    responseType: "token",
    redirectUri: "http://localhost:4200/dashboard",
    audience: "http://localhost:50138/api",
    scope: "openid profile read:to-do-list remove:to-do-list read:to-do-item write:to-do-item remove:to-do-item write:to-do-list"
  }
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
