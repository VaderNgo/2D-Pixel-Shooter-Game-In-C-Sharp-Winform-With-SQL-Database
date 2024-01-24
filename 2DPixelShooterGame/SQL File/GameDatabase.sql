CREATE DATABASE GAME 
USE GAME
CREATE TABLE PLAYERS(
	PLAYER_ID VARCHAR(3) CONSTRAINT PLAYERS_PK PRIMARY KEY,
	PLAYER_NAME VARCHAR(20),
	HP INT DEFAULT(100),
	AMMO INT DEFAULT(0),
	GOLD INT DEFAULT(0),
	CREATED_AT SMALLDATETIME,
)

CREATE TABLE MAPS
(
	MAP_ID VARCHAR(3) PRIMARY KEY,
	MAP_NAME VARCHAR (20)
)
INSERT INTO MAPS VALUES ('M01','Ruin Street')

CREATE TABLE PLAY
(
	PLAYER_ID VARCHAR(3),
	MAP_ID VARCHAR(3),
	STAGE INT DEFAULT (1),
	DIFFICULTY INT,
	ISVICTORY CHAR(1),
	ATTEMPT INT,
	CONSTRAINT PLAY_PK PRIMARY KEY (PLAYER_ID,MAP_ID),
	CONSTRAINT PLAY_PID_FK FOREIGN KEY (PLAYER_ID) REFERENCES PLAYERS(PLAYER_ID),
	CONSTRAINT PLAY_MID_FK FOREIGN KEY (MAP_ID) REFERENCES MAPS(MAP_ID),
)

CREATE TABLE WEAPONS(
	WEAPON_ID VARCHAR(3) CONSTRAINT WEAPONS_PK PRIMARY KEY,
	WEAPON_NAME VARCHAR(20),
	DAMAGE INT,
	PRICE INT,
)
INSERT INTO WEAPONS VALUES('W01','Knife',1,0);
INSERT INTO WEAPONS VALUES('W02','Pistol',2,2);
INSERT INTO WEAPONS VALUES('W03','AK-47',5,5);
INSERT INTO WEAPONS VALUES('W04','RPG',10,10);
INSERT INTO WEAPONS VALUES('W05','Bazooka',50,50);

CREATE TABLE ITEMS(
	ITEM_ID VARCHAR(3) CONSTRAINT ITEMS_PK PRIMARY KEY,
	ITEM_NAME VARCHAR(20),
	ITEM_INFO VARCHAR(1000),
	PRICE INT
)
INSERT INTO ITEMS VALUES ('I01','Ammo Pack 1','+5 Ammo',1)
INSERT INTO ITEMS VALUES ('I02','Ammo Pack 2','+20 Ammo',3)
INSERT INTO ITEMS VALUES ('I03','Speed Potion','+1 Speed',10)
INSERT INTO ITEMS VALUES ('I04','Health Potion','+10 HP',20)

CREATE TABLE INVENTORY
(
	PLAYER_ID VARCHAR (3),
	ITEM_ID VARCHAR (3),
	QUANTITY INT
	CONSTRAINT INVENTORY_PK PRIMARY KEY (PLAYER_ID,ITEM_ID),
	CONSTRAINT INVENTORY_PID_FK FOREIGN KEY (PLAYER_ID) REFERENCES PLAYERS(PLAYER_ID),
	CONSTRAINT INVENTORY_IID_FK FOREIGN KEY (ITEM_ID) REFERENCES ITEMS(ITEM_ID),
)

CREATE TABLE OWN
(
	PLAYER_ID VARCHAR(3),
	WEAPON_ID VARCHAR(3),
	CONSTRAINT OWN_PK PRIMARY KEY (PLAYER_ID,WEAPON_ID),
	CONSTRAINT OWN_PID_FK FOREIGN KEY (PLAYER_ID) REFERENCES PLAYERS(PLAYER_ID),
	CONSTRAINT OWN_WID_FK FOREIGN KEY (WEAPON_ID) REFERENCES WEAPONS(WEAPON_ID)
)

