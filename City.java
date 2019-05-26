
public class City {
	
	public class Point {
		public int x;
		public int y;
		
		public Point() {}
		
		public Point(int x, int y) {
			this.x = x;
			this.y = y;
		}
	}
	
	Point location;
	
	public City(Point location) {
		this.location = location;
	}
	
	public City(int x, int y) {
		this.location = new Point();
		this.location.x = x;
		this.location.y = y;
	}

	double distanceTo(City city) {
		return Math.sqrt( Math.pow(this.location.x - city.location.x, 2.0) + Math.pow(this.location.y - city.location.y, 2.0));
	}
	
	@Override
    public String toString(){
        return this.location.x + ", " + this.location.y;
    }
}
