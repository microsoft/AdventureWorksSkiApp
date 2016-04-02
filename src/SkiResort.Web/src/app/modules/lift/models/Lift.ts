module SkiResort.App.Lift.Models {
    'use strict';

    export class Lift {
        public liftId: number;
        public name: string;
        public rating: LiftRating;
        public status: LiftStatus;
        public latitude: number;
        public longitude: number;
        public stayAway: boolean; // Danger
        public waitingTime: number;
        public closedReason: string;

        public serialize(data): Lift {
            this.liftId = data.liftId;
            this.name = data.name;
            this.rating = data.rating;
            this.status = data.status;
            this.latitude = data.latitude;
            this.longitude = data.longitude;
            this.stayAway = data.stayAway;
            this.waitingTime = data.waitingTime;
            this.closedReason = data.closedReason;

            return this;
        }

        public deserialize() {
            return {
                liftId: this.liftId,
                name: this.name,
                rating: this.rating,
                status: this.status,
                latitude: this.latitude,
                longitude: this.longitude,
                stayAway: this.stayAway,
                waitingTime: this.waitingTime,
                closedReason: this.closedReason
            };
        }
    }
}
