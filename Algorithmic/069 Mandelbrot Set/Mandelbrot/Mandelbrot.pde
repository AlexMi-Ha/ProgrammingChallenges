// gets scaled as you scroll in
int MAX_ITER = 150;
int animateScroll = 0;
boolean saveFrameSwitch = false;

void setup() {
  size(500,500);
}

void draw() {
  background(255);
  calcPixel();
  if(animateScroll != 0) {
    scaleScreen(animateScroll);
  }
  if(saveFrameSwitch) {
    saveFrame();
  }
}

void calcPixel() {
  for(int x = 0; x < width; ++x) {
    for(int y = 0; y < height; ++y) {
      int iter = mandelbrot(x,y);
      color c = getColor(iter, true);
      set(x,y, c);
    }
  }
}

color getColor(int iteration, boolean colored) {
  if(!colored) {
    return color((int)(( (MAX_ITER - iteration) / (double)MAX_ITER ) * 255));
  }
  int hue = (int)(255 * (double)iteration / MAX_ITER);
  int value = iteration < MAX_ITER ? 255 : 0;
  colorMode(HSB);
  return color(hue, 255, value);
}

double reStart = -2;
double reEnd = 1;
double imagStart =-1.5;
double imagEnd= 1.5;

int mandelbrot(double x, double y) {
  
  // scale
  x = reStart + (x / width) * (reEnd - reStart);
  y = imagStart + (y / height) * (imagEnd - imagStart);
  
  //z_n+1 = z_n^2 + c
  
  // z_n^2 = (real+i*imag) * (real+i*imag) = real^2 + 2*i*imag*real - imag^2
  // real(z_n^2) = real^2 - imag^2
  // imag(z_n^2) = 2*imag*real
  double zReal = 0;
  double zImag = 0;
  
  int iter = 0;
  while(iter < MAX_ITER) {
    if(absImag(zReal, zImag) >= 2) {
      break;
    }
    double real = Math.pow(zReal,2) - Math.pow(zImag,2);
    zImag = 2*zImag*zReal + y;
    zReal = real + x;
    iter++;
  }
  return iter;
}

double absImag(double real, double imag) {
  return Math.sqrt(real*real + imag*imag);
}

void keyPressed() {
  if(key == '+') {
    animateScroll = -1;
  }else if(key == '-') {
    animateScroll = 1;
  }
}

void keyReleased() {
  animateScroll = 0;
}

void mouseWheel(MouseEvent event) {
  float e = event.getCount();
  scaleScreen((int)e);
}

void scaleScreen(int deltaScale) {
  MAX_ITER += -deltaScale;
  
  double midpointX = (reEnd + reStart)/2d;
  double midpointY = (imagEnd + imagStart)/2d;
  double newSpan = Math.abs(reEnd-reStart) * (1+deltaScale*0.1);
  
  reStart = midpointX - newSpan / 2d;
  reEnd = midpointX + newSpan / 2d;
  
  imagStart = midpointY - newSpan / 2d;
  imagEnd = midpointY + newSpan / 2d;
}

int xOffset, yOffset;
void mousePressed() {
  xOffset = mouseX; 
  yOffset = mouseY; 

}

void mouseDragged() {
  double dx = ((double)xOffset - mouseX) / width * Math.abs(reEnd - reStart);
  double dy = ((double)yOffset - mouseY) / width * Math.abs(reEnd - reStart);
  reStart += dx;
  reEnd += dx;
  
  imagStart += dy;
  imagEnd += dy;
  
  xOffset = mouseX;
  yOffset = mouseY;
}
