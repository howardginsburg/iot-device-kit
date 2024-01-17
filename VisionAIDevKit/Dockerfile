# Use an official Python runtime as a parent image
FROM arm32v7/python:3.8-slim-buster

# Set Python to run in unbuffered mode
ENV PYTHONUNBUFFERED=1

# Set the working directory in the container to /app
WORKDIR /app

# Add the current directory contents into the container at /app
ADD . /app

# Install any needed packages specified in requirements.txt
#RUN pip install --no-cache-dir requests==2.26.0
RUN pip install requests==2.26.0

# Run the command to start your application
CMD ["python", "./rtspenabler.py"]