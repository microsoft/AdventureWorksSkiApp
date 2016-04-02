module SkiResort.App.Rental.Models {
    'use strict';

    export class Rental {
        public rentalId: number;
        public userEmail: string;
        public startDate: Date;
        public endDate: Date;
        public pickupHour: number;
        public activity: RentalActivity;
        public category: RentalCategory;
        public goal: RentalGoal;
        public shoeSize: number;
        public skiSize: number;
        public poleSize: number;
        public totalCost: number;

        public serialize(data): Rental {
            this.rentalId = data.rentalId;
            this.userEmail = data.userEmail;
            this.startDate = new Date(data.startDate);
            this.endDate = new Date(data.endDate);
            this.pickupHour = data.pickupHour;
            this.activity = data.activity;
            this.category = data.category;
            this.goal = data.goal;
            this.shoeSize = data.shoeSize;
            this.skiSize = data.skiSize;
            this.poleSize = data.poleSize;
            this.totalCost = data.totalCost;

            return this;
        }

        public deserialize() {
            return {
                rentalId: this.rentalId,
                userEmail: this.userEmail,
                startDate: this.startDate.toJSON(),
                endDate: this.endDate.toJSON(),
                pickupHour: this.pickupHour,
                activity: this.activity,
                category: this.category,
                goal: this.goal,
                shoeSize: this.shoeSize,
                skiSize: this.skiSize,
                poleSize: this.poleSize,
                totalCost: this.totalCost
            };
        }
    }
}
