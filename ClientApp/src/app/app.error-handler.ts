import { ErrorHandler, NgZone, isDevMode } from "@angular/core";
import { ToastrService } from "ngx-toastr";
import * as Sentry from "@sentry/browser";

export class AppErrorHandler implements ErrorHandler{
    constructor(
        private ngZone: NgZone,
        private toastr: ToastrService){

    }
    
    handleError(error: any): void {

        if(!isDevMode())
            Sentry.captureException(error.originalError || error);
        else
            throw error;
        this.ngZone.run(() =>{
            this.toastr.error('An unexpected error has happened.', 'Error', {
                timeOut: 3000
              });
        });
   
    }
}