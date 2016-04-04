module SkiResort.App.Auth.Models {
    'use strict';

    export interface User {
        id: string;
        userName: string;
        fullName: string;
        email: string;
        photo: string;
    }
}
