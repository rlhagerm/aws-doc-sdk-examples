// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
// SPDX-License-Identifier: Apache-2.0

// Package demotools provides a set of tools that help you write code examples.
//
// **Questioner**
//
// The questioner is used in interactive examples to ask for input from the user
// at a command prompt, validate the answer, and ask the question again, if needed.
// It is exposed through an interface so that it can be mocked for unit testing.
// A pre-written mock is provided in the testtools package.
package demotools

import (
	"bufio"
	"fmt"
	"os"
	"strconv"
	"strings"
	"syscall"

	"golang.org/x/term"
)

// IAnswerValidator defines an interface that validates string input.
type IAnswerValidator interface {
	IsValid(string) bool
}

// NotEmpty is a validator that requires that the input be non-empty.
type NotEmpty struct{}

func (notEmpty NotEmpty) IsValid(answer string) bool {
	valid := len(answer) > 0
	if !valid {
		fmt.Println("I need an answer. Please?")
	}
	return valid
}

// InIntRange is a validator that verifies that the input is an int in the specified
// range (inclusive).
type InIntRange struct {
	Lower int
	Upper int
}

func (inRange InIntRange) IsValid(answer string) bool {
	var valid bool
	intAnswer, err := strconv.Atoi(answer)
	if err != nil {
		valid = false
	} else {
		valid = inRange.Lower <= intAnswer && intAnswer <= inRange.Upper
	}
	if !valid {
		fmt.Printf("%v must be an integer between %v and %v.\n", answer, inRange.Lower, inRange.Upper)
	}
	return valid
}

// InFloatRange is a validator that verifies that the input is a float in the specified
// range (inclusive).
type InFloatRange struct {
	Lower float64
	Upper float64
}

func (inRange InFloatRange) IsValid(answer string) bool {
	var valid bool
	floatAnswer, err := strconv.ParseFloat(answer, 32)
	if err != nil {
		valid = false
	} else {
		valid = inRange.Lower <= floatAnswer && floatAnswer <= inRange.Upper
	}
	if !valid {
		fmt.Printf("%v must be a number between %v and %v.\n", answer, inRange.Lower, inRange.Upper)
	}
	return valid
}

// IQuestioner is an interface that asks questions at a command prompt and validates
// the answers.
type IQuestioner interface {
	Ask(question string, validators ...IAnswerValidator) string
	AskBool(question string, expected string) bool
	AskInt(question string, validators ...IAnswerValidator) int
	AskFloat64(question string, validators ...IAnswerValidator) float64
	AskChoice(question string, choices []string) int
	AskPassword(question string, minLength int) string
}

// Questioner implements IQuestioner and stores input in a reader.
type Questioner struct {
	reader *bufio.Reader
}

// NewQuestioner returns a Questioner that is initialized with a reader.
func NewQuestioner() *Questioner {
	questioner := Questioner{reader: bufio.NewReader(os.Stdin)}
	return &questioner
}

// Ask asks the specified question at a command prompt and validates the answer with
// the specified set of validators. If the answer fails validation, the question is
// asked again. The answer is trimmed of whitespace, but is otherwise not altered.
// When no validators are specified, the questioner waits for any answer and then
// continues.
func (questioner Questioner) Ask(question string, validators ...IAnswerValidator) string {
	var answer string
	var err error
	isValid := false
	for !isValid {
		fmt.Println(question)
		answer, err = questioner.reader.ReadString('\n')
		if err != nil {
			panic(err)
		} else {
			answer = strings.TrimSpace(answer)
			isValid = true
			for _, validator := range validators {
				isValid = validator.IsValid(answer)
				if !isValid {
					break
				}
			}
		}
	}
	return answer
}

// AskBool asks a question with an expected answer. If the expected answer is given,
// it returns true; otherwise, it returns false.
func (questioner Questioner) AskBool(question string, expected string) bool {
	answer := questioner.Ask(question, NotEmpty{})
	return strings.ToLower(answer) == expected
}

// AskInt asks a question and converts the answer to an int. If the answer cannot be
// converted, the function panics.
func (questioner Questioner) AskInt(question string, validators ...IAnswerValidator) int {
	answer := questioner.Ask(question, validators...)
	answerInt, err := strconv.Atoi(answer)
	if err != nil {
		panic(err)
	}
	return answerInt
}

// AskFloat64 asks a question and converts the answer to a float64. If the answer cannot
// be converted, the function panics.
func (questioner Questioner) AskFloat64(question string, validators ...IAnswerValidator) float64 {
	answer := questioner.Ask(question, validators...)
	answerFloat, err := strconv.ParseFloat(answer, 64)
	if err != nil {
		panic(err)
	}
	return answerFloat
}

func (questioner Questioner) AskChoice(question string, choices []string) int {
	for index, choice := range choices {
		question += fmt.Sprintf("\t%v. %v\n", index+1, choice)
	}
	question += fmt.Sprintf("Enter a choice between %v and %v: ", 1, len(choices))
	answer := questioner.AskInt(question, InIntRange{Lower: 1, Upper: len(choices)})
	return answer - 1
}

func (questioner Questioner) AskPassword(question string, minLength int) string {
	var bpw []byte
	var err error
	valid := false
	for !valid {
		fmt.Println(question)
		bpw, err = term.ReadPassword(int(syscall.Stdin))
		if err != nil {
			panic(err)
		}
		if len(bpw) > minLength {
			valid = true
		} else {
			fmt.Printf("The password you entered isn't valid. It has to be at least %v characters.\n", minLength)
		}
	}
	return string(bpw)
}
