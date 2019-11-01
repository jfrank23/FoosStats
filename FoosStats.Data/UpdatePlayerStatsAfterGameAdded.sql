CREATE TRIGGER addGame AFTER INSERT ON Games
BEGIN
	UPDATE Players
	SET 
		GoalsFor = GoalsFor + New.BlueScore, 
		GoalsAgainst = GoalsAgainst + New.RedScore,
		GamesPlayed =GamesPlayed +1,
		GamesWon = CASE WHEN New.BlueScore=10 THEN GamesWon +1 ELSE GamesWon +0 END,
		GamesLost = CASE WHEN New.RedScore=10 THEN GamesLost +1 ELSE GamesLost +0 END
	WHERE PlayerId = New.BlueDefense OR PlayerId = New.BlueOffense;
	UPDATE Players
	SET 
		GoalsFor = GoalsFor + New.RedScore, 
		GoalsAgainst = GoalsAgainst + New.BlueScore,
		GamesPlayed =GamesPlayed +1,
		GamesWon = CASE WHEN New.RedScore=10 THEN GamesWon +1 ELSE GamesWon +0 END,
		GamesLost = CASE WHEN New.BlueScore=10 THEN GamesLost +1 ELSE GamesLost +0 END
	WHERE PlayerId = New.RedDefense OR PlayerId = New.RedOffense;
END;
		