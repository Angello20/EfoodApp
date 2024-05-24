# Dagster & Meltano
This project uses Dagster to automate the execution of a Meltano pipeline consisting of CSV extractors and loaders. The main goal is to streamline the scheduling and execution of this workflow efficiently.

## Requirements

- Python 3.x installed on your system.
- Meltano installed in your system.
- Dagster 1.5.6

## Getting started

1. Create the .env based on .env.example and add your meltano project path

2. Go to meltano

```bash
cd dagster_project
```

3. Change file_de.json and add the path where `input/CSV-Template1.csv` is located

"path": "CSV_FILE_PATH",

4. Go to dagster_project

```bash
cd dagster_project
```

5. Install the dependencies

```bash
pip install -e ".[dev]"
```

6. Start Dagster

```bash
dagster dev
```

## Results

- This is the dashboard with the meltano job

![Screenshot (160)](https://github.com/Bryancampos20/Dagster-Meltano/blob/main/results/1.png)

- The only way to execute the job is by clicking it

![Screenshot (160)](https://github.com/Bryancampos20/Dagster-Meltano/blob/main/results/2.png)

- When we click on "Launch Run" we can execute the job

![Screenshot (160)](https://github.com/Bryancampos20/Dagster-Meltano/blob/main/results/3.png)

- This is the result

![Screenshot (160)](https://github.com/Bryancampos20/Dagster-Meltano/blob/main/results/4.png)

This is a basic example of how we can run a Meltano command in Dagster.

Happy coding! 🚀
