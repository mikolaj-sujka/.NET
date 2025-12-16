using TheDependencyProblem.CarExample;

Console.WriteLine();

var dieselCar = new Car(new DieselEngine());
dieselCar.StartEngine();

var petrolCar = new Car(new PetrolEngine());
petrolCar.StartEngine();

Console.WriteLine();
