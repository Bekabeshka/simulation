import java.util.Collections;
import java.util.List;
import java.util.Scanner;

public class Tester {

	public static double count(int energy, int newEnergy, double temperature) {
        if (newEnergy < energy) {
            return 1.0;
        }
        return Math.exp(-(energy - newEnergy) / temperature);
    }
	
	public static void main(String[] args) {


        Tour.destinationCities.add(new City(60, 200));
        Tour.destinationCities.add(new City(180, 200));
        Tour.destinationCities.add(new City(80, 180));
        Tour.destinationCities.add(new City(140, 180));
        Tour.destinationCities.add(new City(20, 160));
        Tour.destinationCities.add(new City(100, 160));
        Tour.destinationCities.add(new City(200, 160));
        Tour.destinationCities.add(new City(140, 140));
        Tour.destinationCities.add(new City(40, 120));
        Tour.destinationCities.add(new City(100, 120));

        double temp = 10000;

        double alpha = 5;

        Tour currentSolution = new Tour();
        currentSolution.generateIndividual();
        
        System.out.println("Initial distance: " + currentSolution.getDistance());

        Tour best = new Tour(currentSolution.getTour());
        
        while (temp > 100) {
            Tour newSolution = new Tour(currentSolution.getTour());

            int tourPos1 = (int) (newSolution.tourSize() * Math.random());
            int tourPos2 = (int) (newSolution.tourSize() * Math.random());

            City citySwap1 = newSolution.getCity(tourPos1);
            City citySwap2 = newSolution.getCity(tourPos2);

            newSolution.setCity(tourPos2, citySwap1);
            newSolution.setCity(tourPos1, citySwap2);
            
            int L = currentSolution.getDistance();
            int L_ = newSolution.getDistance();

            if (count(L, L_, temp) > Math.random()) {
                currentSolution = new Tour(newSolution.getTour());
            }

            if (currentSolution.getDistance() < best.getDistance()) {
                best = new Tour(currentSolution.getTour());
            }
            
            temp = temp - alpha;
        }

        System.out.println("Final distance: " + best.getDistance());
        System.out.println("Tour: " + best);
    }
}

