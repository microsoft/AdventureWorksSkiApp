library(rpart.plot)

serverData = RxSqlServerData(sqlQuery = "SELECT * FROM RentalFeatures ORDER BY Year, Month, Day", connectionString = "Driver=SQL Server;Server=vm75bolg55sbwx2.westeurope.cloudapp.azure.com;Database=AdventureWorks.SkiResort;Uid=skiresort;Pwd=P2ssw0rd@1;")

rentals = rxImport(serverData)

head(rentals)

plot(factor(rentals$Snow), rentals$RentalCount)
plot(factor(rentals$WeekDay), rentals$RentalCount)

rentals$FHoliday = factor(rentals$Holiday)
rentals$FSnow = factor(rentals$Snow)
rentals$FWeekDay = factor(rentals$WeekDay)

plot(ISOdate(rentals$Year, rentals$Month, rentals$Day), rentals$RentalCount)
train_data = rentals[rentals$Year < 2015,]
test_data = rentals[rentals$Year == 2015,]
test_counts = test_data$RentalCount

model = rpart(RentalCount ~ Month + Day + FWeekDay + FSnow + FHoliday, train_data, )

prp(model)

predict(model, data.frame(Month = 1, Day = 1, FWeekDay = factor(7), FHoliday = factor(FALSE), FSnow = factor(TRUE)))
p = predict(model, test_data)
plot(p - test_counts)
mean((p - test_counts) ^ 2)
