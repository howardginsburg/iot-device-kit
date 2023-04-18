int RED_LED = 20;
int GREEN_LED = 19;
int BLUE_LED = 39;
void setup() {
  // initialize the pins as digital output.
  pinMode(RED_LED, OUTPUT);
  pinMode(GREEN_LED, OUTPUT);
  pinMode(BLUE_LED, OUTPUT);
}
void loop() {
  // turn red LED on
  digitalWrite(RED_LED, HIGH);
  delay(100);
  // turn red LED off
  digitalWrite(RED_LED, LOW);
  delay(100);
  // turn green LED on
  digitalWrite(GREEN_LED, HIGH);
  delay(100);
  // turn green LED off
  digitalWrite(GREEN_LED, LOW);
  delay(100);
  // turn blue LED on
  digitalWrite(BLUE_LED, HIGH);
  delay(100);
  // turn blue LED off
  digitalWrite(BLUE_LED, LOW);
  delay(100);
}