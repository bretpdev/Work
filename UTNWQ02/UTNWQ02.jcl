#UTNWQ02.jcl

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWQ02.NWQ02R1
then
rm ${reportdir}/UNWQ02.NWQ02R1
fi
if test -a ${reportdir}/UNWQ02.NWQ02R2
then
rm ${reportdir}/UNWQ02.NWQ02R2
fi
if test -a ${reportdir}/UNWQ02.NWQ02R3
then
rm ${reportdir}/UNWQ02.NWQ02R3
fi
if test -a ${reportdir}/UNWQ02.NWQ02R4
then
rm ${reportdir}/UNWQ02.NWQ02R4
fi
if test -a ${reportdir}/UNWQ02.NWQ02R5
then
rm ${reportdir}/UNWQ02.NWQ02R5
fi
if test -a ${reportdir}/UNWQ02.NWQ02R6
then
rm ${reportdir}/UNWQ02.NWQ02R6
fi

# run the program

sas ${codedir}/UTNWQ02.sas -log ${reportdir}/UNWQ02.NWQ02R1  -mautosource
