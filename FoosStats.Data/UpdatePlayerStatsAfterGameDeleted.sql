CREATE TRIGGER deleteGame AFTER DELETE ON Games
BEGIN
	UPDATE Players
	SET 
		GoalsFor = GoalsFor - Old.BlueScore, 
		GoalsAgainst = GoalsAgainst - Old.RedScore,
		GamesPlayed =GamesPlayed -1,
		GamesWon = CASE WHEN Old.BlueScore=10 THEN GamesWon -1 ELSE GamesWon END,
		GamesLost = CASE WHEN Old.RedScore=10 THEN GamesLost -1 ELSE GamesLost END
	WHERE PlayerId = Old.BlueDefense OR PlayerId = Old.BlueOffense;
	UPDATE Players
	SET 
		GoalsFor = GoalsFor - Old.RedScore, 
		GoalsAgainst = GoalsAgainst - Old.BlueScore,
		GamesPlayed =GamesPlayed -1,
		GamesWon = CASE WHEN Old.RedScore=10 THEN GamesWon -1 ELSE GamesWon +0  END,
		GamesLost = CASE WHEN Old.BlueScore=10 THEN GamesLost -1 ELSE GamesLost +0  END
	WHERE PlayerId =Old.RedDefense OR PlayerId = Old.RedOffense;
END;