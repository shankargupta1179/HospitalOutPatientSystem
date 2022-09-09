CREATE DATABASE  IF NOT EXISTS `hospital_management_system`
USE `hospital_management_system`;

CREATE TABLE `patient_records` IF NOT EXISTS(
  `patient_id` int NOT NULL,
  `patient_name` varchar(45) DEFAULT NULL,
  `patient_age` int DEFAULT NULL,
  `patient_symptoms` varchar(45) DEFAULT NULL,
  `patient_severity` int DEFAULT NULL,
  `patient_feedback` varchar(45) DEFAULT NULL,
  `patient_prescription` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`patient_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
